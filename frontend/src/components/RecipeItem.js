import React, { useContext } from "react";
import "./RecipeItem.css";
import { useNavigate } from "react-router-dom";
import { ApiContext } from "../contexts/ApiContext";

const RecipeItem = (props) => {
  const { recipe } = props;

  const { imagesApiUrl } = useContext(ApiContext);
  const navigate = useNavigate();

  const openRecipe = () => {
    navigate(`/recipes/${recipe.id}`);
  };

  return (
    <div onClick={openRecipe} className="recipe-preview">
      <p className="preview-name">{recipe.name}</p>
      <img
        alt="Recipe preview"
        className="preview-image"
        src={imagesApiUrl + recipe.mainImagePath}
      />
      <p className="preview-description">{recipe.description}</p>
    </div>
  );
};

export default RecipeItem;
