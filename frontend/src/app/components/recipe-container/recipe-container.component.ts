import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { RecipeView } from 'src/app/models/Recipe';
import { RecipeService } from 'src/app/services/recipe.service';

@Component({
  selector: 'app-recipe-container',
  templateUrl: './recipe-container.component.html',
  styleUrls: ['./recipe-container.component.css'],
})
export class RecipeContainerComponent implements OnInit {
  recipes: RecipeView[] = [];

  constructor(private recipeService: RecipeService) {}

  ngOnInit(): void {
    this.recipeService.getRecipes().subscribe((recipes) => {
      this.recipes = recipes;
    });
  }
}
