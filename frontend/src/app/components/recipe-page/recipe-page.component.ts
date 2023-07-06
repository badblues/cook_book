import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { lastValueFrom } from 'rxjs';
import { RecipeView } from 'src/app/models/Recipe';
import { RecipeService } from 'src/app/services/recipe.service';

@Component({
  selector: 'app-recipe-page',
  templateUrl: './recipe-page.component.html',
  styleUrls: ['./recipe-page.component.css'],
})
export class RecipePageComponent implements OnInit {
  
  recipe!: RecipeView;

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    this.recipe = this.route.snapshot.data['recipe'][0];
    console.log(this.recipe)
  }
}
