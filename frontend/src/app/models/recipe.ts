export interface Recipe {
  id?: string;
  name: string;
  description: string;
  mainImagePath: string;
  stepsImagePathsAndDescriptions: [string, string][];
}