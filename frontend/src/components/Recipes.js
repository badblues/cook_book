import React, { useState, useContext, useEffect } from "react";
import "./Recipes.css";
import { ApiContext } from "../contexts/ApiContext";
import RecipeItem from "./RecipeItem";
import FakeRecipeItem from "./FakeRecipeItem";

const Recipes = () => {
  const [recipes, setRecipes] = useState([]);
  const [loading, setLoading] = useState(true);

  const { recipeApiService } = useContext(ApiContext);

  const fetchData = async () => {
    try {
      const recipes = await recipeApiService.getRecipes();
      setRecipes(recipes);
      setLoading(false);
    } catch (error) {
      console.log(error);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  if (loading) {
    return(
    <div className="recipe-previews-container">
      <FakeRecipeItem />
      <FakeRecipeItem />
      <FakeRecipeItem />
      <FakeRecipeItem />
      <FakeRecipeItem />
      <FakeRecipeItem />
    </div>
    );
  }

  return (
    <div className="recipe-previews-container">
      {recipes.map((recipe) => (
        <RecipeItem recipe={recipe} />
      ))}
    </div>
  );
};

export default Recipes;
