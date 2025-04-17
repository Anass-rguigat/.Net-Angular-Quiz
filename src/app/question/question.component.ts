
import { Component, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { QuestionService } from '../service/question.service';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-question',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './question.component.html',
  styleUrl: './question.component.scss'
})
export class QuestionComponent implements OnDestroy {
  public name: string = "";
  public questionList: any = [];
  public currentQuestion: number = 0;
  public userAnswers: any = [];
  public isQuizStarted: boolean = false;
  public isQuizCompleted: boolean = false;
  public showQuestions: boolean = false;
  public score: number = 0;

  public timePerQuestion: number = 40; // Durée en secondes
  public timeLeft: number = this.timePerQuestion;
  private timer: any;

  constructor(private questionService: QuestionService) {}

  ngOnInit(): void {
    this.name = localStorage.getItem("name")!;
    this.getAllQuestions();
  }

  getAllQuestions() {
    this.questionService.getQuestionJson()
      .subscribe(res => {
        this.questionList = res.questions;
      });
  }

  startQuiz() {
    this.isQuizStarted = true;
    this.showQuestions = false;
    this.currentQuestion = 0;
    this.userAnswers = [];
    this.isQuizCompleted = false;
    this.score = 0;
    this.startTimer();
  }

  nextQuestion() {
    if (!this.userAnswers[this.currentQuestion]) {
      this.userAnswers[this.currentQuestion] = null;
    }

    // Si l'utilisateur est arrivé à la dernière question, on calcule son score
    if (this.currentQuestion < this.questionList.length - 1) {
      this.currentQuestion++;
    } else {
      this.calculateScore();
      this.isQuizCompleted = true;
    }

    // Arrêter et redémarrer le timer pour la prochaine question
    this.stopTimer();
    this.startTimer();
  }


  previousQuestion() {
    if (this.currentQuestion > 0) {
      this.stopTimer();
      this.currentQuestion--;
      this.startTimer();
    }
  }

  calculateScore() {
    this.stopTimer();
    this.score = 0;
    this.questionList.forEach((question: any, index: number) => {
      const correctAnswer = question.options.find((opt: any) => opt.correct);
      const userAnswer = this.userAnswers[index];

      if (userAnswer && userAnswer.text === correctAnswer.text) {
        this.score++;
        this.userAnswers[index].correct = true;
      } else {
        this.userAnswers[index] = {
          ...(userAnswer || { text: "Aucune réponse" }),
          correct: false
        };
      }
    });
  }

  getCorrectAnswer(question: any) {
    return question.options.find((opt: any) => opt.correct).text;
  }

  restartQuiz() {
    this.stopTimer();
    this.isQuizStarted = false;
    this.isQuizCompleted = false;
    this.showQuestions = false;
    this.currentQuestion = 0;
    this.userAnswers = [];
    this.score = 0;
  }

  startTimer() {
    this.timeLeft = this.timePerQuestion;
    this.timer = setInterval(() => {
      this.timeLeft--;
      if (this.timeLeft === 0) {
        this.nextQuestion(); // Passe automatiquement à la question suivante
      }
    }, 1000);
  }

  stopTimer() {
    if (this.timer) {
      clearInterval(this.timer);
    }
  }

  ngOnDestroy() {
    this.stopTimer();
  }
}
