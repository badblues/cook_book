import React, { useEffect, useState, useContext } from "react";
import "./RecipePage.css";
import { useParams } from "react-router-dom";
import { ApiContext } from "../contexts/ApiContext";

const RecipePage = () => {
  const { id } = useParams();

  const [recipe, setRecipe] = useState(null);
  const [loading, setLoading] = useState(true);

  const { recipeApiService, imagesApiUrl } = useContext(ApiContext);

  const fetchData = async () => {
    try {
      const recipe = await recipeApiService.getRecipe(id);
      setRecipe(recipe);
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
    <div className="recipe-container">
      <p className="recipe-summary-name">{recipe.name}</p>
      <div className="recipe-summary-container">
        <img
          alt="Recipe image"
          className="recipe-summary-image"
          src={imagesApiUrl + recipe.mainImagePath}
        />
        <p className="recipe-summary-description">{recipe.description}</p>
      </div>
      {recipe.stepsImagesPaths.map((imagePath, index) => (
        <div className="step-container">
          <img
            alt={`Recipe step #${index}`}
            className="step-image"
            src={imagesApiUrl + imagePath}
          />
          <p className="step-description">{recipe.stepsTexts[index]}</p>
        </div>
      ))}
    </div>
  );
};

export default RecipePage;
