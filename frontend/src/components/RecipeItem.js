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
      <label>{recipe.name}</label>
      <img
        alt="Recipe preview"
        width="300px"
        src={imagesApiUrl + recipe.mainImagePath}
      />
      <label>{recipe.description}</label>
    </div>
  );
};

export default RecipeItem;
