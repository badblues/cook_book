import React from "react";

const RecipeItem = (props) => {
  const { recipe } = props;

  return <div>{recipe.name}</div>;
};

export default RecipeItem;
