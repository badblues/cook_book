import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { RecipeData, RecipeView } from '../models/Recipe';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
  private apiUrl: string = "http://localhost:5055/recipes";

  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  constructor(private http: HttpClient) { }

  createRecipe(recipe: RecipeData): Observable<RecipeView> {
    console.log(recipe);
    return this.http.post<RecipeView>(this.apiUrl, recipe, this.httpOptions);
  }

  getRecipe(id: string): Observable<RecipeView> {
    let url = `${this.apiUrl}?id=${id}`;
    return this.http.get<RecipeView>(url);
  }

  getRecipes(): Observable<RecipeView[]> {
    return this.http.get<RecipeView[]>(this.apiUrl);
  }

  updateRecipe(recipe: RecipeData): void {
    let id: string = recipe.id!;
    let url: string = `${this.apiUrl}?id=${id}`;
    this.http.put<RecipeView>(url, recipe, this.httpOptions);
  }

  deleteRecipe(id: string): void {
    let url: string = `${this.apiUrl}?id=${id}`;
    this.http.delete<any>(url);
  }

}
