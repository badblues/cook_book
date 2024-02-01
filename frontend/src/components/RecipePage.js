import React, { useEffect, useState, useContext } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { ApiContext } from "../contexts/ApiContext";
import "./RecipePage.css";
import { toast } from "react-toastify";

const RecipePage = () => {
  const { id } = useParams();
  const [recipe, setRecipe] = useState(null);
  const [loading, setLoading] = useState(true);
  const { recipeApiService, imagesApiUrl } = useContext(ApiContext);
  const navigate = useNavigate();

  const fetchData = async () => {
    try {
      const recipe = await recipeApiService.getRecipe(id);
      setRecipe(recipe);
      setLoading(false);
    } catch (error) {
      if (error.response?.status === 400 || error.response?.status === 404) {
        navigate("/not-found");
      }
      console.log(error);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const removeRecipe = async () => {
    await recipeApiService.removeRecipe(id);
    toast.success(`Recipe deleted`);
    navigate("/");
  };

  if (loading) {
    return <div>LOADING</div>;
  }

  return (
    <div className="recipe-container">
      <div className="recipe-header">
        <h1 className="recipe-summary-name">{recipe.name}</h1>
        <div className="buttons-panel">
          <button
            className="default-button"
            onClick={removeRecipe}>
            REMOVE RECIPE
          </button>
        </div>
      </div>
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
