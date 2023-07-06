import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { RecipeView } from 'src/app/models/Recipe';

@Component({
  selector: 'app-recipe-item',
  templateUrl: './recipe-item.component.html',
  styleUrls: ['./recipe-item.component.css']
})
export class RecipeItemComponent {

  apiUrl: string = "http://localhost:5055/images/";

  @Input() recipe!: RecipeView;

  constructor(private router: Router) {}

  navigateToRecipePage() {
    this.router.navigate([`recipe/${this.recipe.id}`]);
  }
}
