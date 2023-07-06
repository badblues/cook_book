import { Injectable } from '@angular/core';
import {
  Router,
  Resolve,
  RouterStateSnapshot,
  ActivatedRouteSnapshot,
} from '@angular/router';
import { Observable, delay } from 'rxjs';
import { RecipeView } from '../models/Recipe';
import { RecipeService } from '../services/recipe.service';

@Injectable({
  providedIn: 'root',
})
export class RecipeResolver implements Resolve<Observable<RecipeView>> {
  constructor(private router: Router, private recipeService: RecipeService) {}

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<RecipeView> {
    let id = route.paramMap.get('id');
    if (id == null) {
      this.router.navigate(['']);
      console.error("id is bad");
      //return new Observable({name: "", description: "", stepsTexts: []});
    }
    console.log(`resolving shit, id = ${id}`);
    return this.recipeService.getRecipe(id!).pipe(delay(500));
  }
}
