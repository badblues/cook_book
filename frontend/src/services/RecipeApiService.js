import http from "axios";

export default class RecipeApiService {

  apiUrl = "http://localhost:5055/recipes";
  
  async getRecipes() {
    let url = this.apiUrl;
    try {
      const response = await http.get(url);
      console.log(response);
      return response.data;
    } catch(error) {
      throw error;
    } 
  }

  async createRecipe(recipe) {
    let url = this.apiUrl;
    try {
      const response = await http.post(url, recipe);
      return response.data;
    } catch(error) {
      throw error;
    } 
  }

  async getRecipe(id) {
    let url = this.apiUrl + `/${id}`;
    try {
      const response = await http.get(url);
      return response.data;
    } catch (error) {
      throw error;
    }
  }



}