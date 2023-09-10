import React, { useState, useContext, useEffect } from "react";
import { ApiContext } from "../contexts/ApiContext";
import RecipeItem from "./RecipeItem";

const Recipes = () => {
  const [recipes, setRecipes] = useState([]);
  const [loading, setLoading] = useState(true);

  const { recipeApiService } = useContext(ApiContext);

  const fetchData = async () => {
    setLoading(true);
    try {
      const recipes = await recipeApiService.getRecipes();
      setRecipes(recipes);
      console.log(recipes);
      setLoading(false);
    } catch (error) {
      console.log(error);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  if (loading) {
    return <div>LOADING</div>;
  }

  return (
    <div>
      {recipes.map((recipe) => (
        <RecipeItem recipe={recipe} />
      ))}
    </div>
  );
};

export default Recipes;
