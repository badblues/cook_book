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
      <label className="name">{recipe.name}</label>
      <div className="preview-container">
        <img
          alt="Recipe preview"
          className="preview-image"
          src={imagesApiUrl + recipe.mainImagePath}
        />
        <label className="description">{recipe.description}</label>
      </div>
      {recipe.stepsImagesPaths.map((imagePath, index) => (
        <div className="step-container">
          <img
            alt={`Recipe step #${index}`}
            className="step-image"
            src={imagesApiUrl + imagePath}
          />
          <label className="step-description">{recipe.stepsTexts[index]}</label>
        </div>
      ))}
    </div>
  );
};

export default RecipePage;
