import { Injectable } from '@angular/core';
import { Recipe } from '../models/recipe';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
  private apiUrl: string = "http://localhost:5055/recipes";

  constructor(private http: HttpClient) { }

  createRecipe(recipe: Recipe): Observable<Recipe> {
    return this.http.post<Recipe>(this.apiUrl, recipe);
  }

  getRecipe(id: string): Observable<Recipe> {
    let url = `${this.apiUrl}?id=${id}`;
    return this.http.get<Recipe>(url);
  }

  getRecipes(): Observable<Recipe[]> {
    return this.http.get<Recipe[]>(this.apiUrl);
  }

  updateRecipe(recipe: Recipe): void {
    let id: string = recipe.id!;
    let url: string = `${this.apiUrl}?id=${id}`;
    this.http.put<Recipe>(url, recipe);
  }
  
  deleteRecipe(id: string): void {
    let url: string = `${this.apiUrl}?id=${id}`;
    this.http.delete<Recipe>(url);
  }

}
