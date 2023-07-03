interface Recipe {
  id?: string;
  name: string;
  description: string;
  stepsTexts: string[];
}

export interface RecipeData extends Recipe{
  mainImageBase64: string;
  stepsImagesBase64: string[];
}

export interface RecipeView extends Recipe {
  mainImagePath: string;
  stepsImagePaths: string[];
}