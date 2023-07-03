import { Component, Output } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { RecipeData } from 'src/app/models/Recipe';
import { RecipeService } from 'src/app/services/recipe.service';

@Component({
  selector: 'app-input-recipe-page',
  templateUrl: './input-recipe-page.component.html',
  styleUrls: ['./input-recipe-page.component.css']
})
export class InputRecipePageComponent {
  name?: string;
  description?: string;
  mainImageBase64?: string;
  stepsImagesBase64: string[] = [];
  stepsTexts: string[] = [];

  mainImage?: File;
  reader: FileReader = new FileReader();
  stepInputs: number = 1;
  stepImages: File[] = [];



  constructor(private recipeService: RecipeService) {
  }

  getCountArray(count: number): number[] {
    return Array(count).fill(0).map((_, index) => index);
  }

  onMainImageSelected(event: any) {
    this.mainImage = event.target.files[0];
    if (this.mainImage)
      this.imageToBase64(this.mainImage)
        .then((base64) => {
          this.mainImageBase64 = base64.substring(23);
          console.log(this.mainImageBase64);
        })
        .catch((error) => console.error(error));
  }

  onStepImageSelected(event: any, index: number) {
    if (index >= this.stepImages.length) {
      this.stepInputs++;
      this.stepImages.push(event.target.files[0]);
      this.stepsImagesBase64.push('');
    }
    else
      this.stepImages[index] = event.target.files[0];
    this.imageToBase64(this.stepImages[index])
      .then((base64) => {
        this.stepsImagesBase64[index] = base64.substring(23);
        console.log(this.mainImageBase64);
      })
      .catch((error) => console.error(error));
  }

  onStepTextChanged(event: any, index: number) {
    if (index >= this.stepsTexts.length)
      this.stepsTexts.push(event.target.value);
    else
      this.stepsTexts[index] = event.target.value;
  }

  imageToBase64(key: File): Promise<string> {
    return new Promise<string>((resolve, reject) => {
      let reader = new FileReader();
      reader.onloadend = () => {
        let base64 = reader.result as string;
        resolve(base64);
      };

      reader.onerror = (error) => {
        reject(error);
      };

      reader.readAsDataURL(key);
    });
  }

  onSubmit() {
    if (!this.name || !this.description || !this.mainImage) {
      alert("Fill the form");
      return;
    }
    if (!this.mainImageBase64) {
      alert("Error");
      return;
    }

    let recipe: RecipeData = {
      name: this.name,
      description: this.description,
      mainImageBase64: this.mainImageBase64,
      stepsImagesBase64: this.stepsImagesBase64,
      stepsTexts: this.stepsTexts
    }

    this.recipeService.createRecipe(recipe).subscribe((response) => console.log(response));
  }
}
