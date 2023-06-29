import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { MainPageComponent } from './components/main-page/main-page.component';
import { RouterModule, Routes } from '@angular/router';
import { HeaderComponent } from './components/header/header.component';
import { FooterComponent } from './components/footer/footer.component';
import { RecipePageComponent } from './components/recipe-page/recipe-page.component';
import { RecipeItemComponent } from './components/recipe-item/recipe-item.component';
import { RecipeContainerComponent } from './components/recipe-container/recipe-container.component';

const appRoutes: Routes = [
  {path: '', component: MainPageComponent},
]

@NgModule({
  declarations: [
    AppComponent,
    MainPageComponent,
    HeaderComponent,
    FooterComponent,
    RecipePageComponent,
    RecipeItemComponent,
    RecipeContainerComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    RouterModule.forRoot(appRoutes),
    HttpClientModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
