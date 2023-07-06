import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InputRecipePageComponent } from './components/input-recipe-page/input-recipe-page.component';
import { MainPageComponent } from './components/main-page/main-page.component';
import { RecipePageComponent } from './components/recipe-page/recipe-page.component';
import { RecipeResolver } from './resolvers/recipe.resolver';

const routes: Routes = [
  { path: '', component: MainPageComponent },
  { path: 'input-recipe', component: InputRecipePageComponent },
  { path: 'recipe/:id', component: RecipePageComponent, resolve: {recipe: RecipeResolver}},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
