import React, { useContext, useState } from "react";
import "./RecipeInput.css";
import { useForm } from "react-hook-form";
import { ApiContext } from "../contexts/ApiContext";

const RecipeInput = () => {
  const { register, handleSubmit, formState } = useForm();
  const { errors } = formState;
  const { recipeApiService } = useContext(ApiContext);
  const [loading, setLoading] = useState(false);
  const [stepCounter, setStepCounter] = useState(1);

  const onSubmit = async (data) => {
    let recipe = {
      name: data.name,
      description: data.description,
      mainImageBase64: await toBase64(data.mainImage),
      stepsImagesBase64: await Promise.all(
        data.stepsImages.map(async (image) => await toBase64(image))
      ),
      stepsTexts: data.stepsDescriptions,
    };
    setLoading(true);
    console.log(recipe);
    try {
      await recipeApiService
        .createRecipe(recipe)
        .then((response) => alert(`Success, ${response.name} created`));
    } catch (error) {
      alert(error?.error);
    } finally {
      setLoading(false);
    }
  };

  const addStep = () => {
    if (stepCounter <= 20) setStepCounter(stepCounter + 1);
  };

  const removeStep = () => {
    if (stepCounter >= 2) setStepCounter(stepCounter - 1);
  };

  const toBase64 = (image) => {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onload = function () {
        resolve(reader.result.substring(23)); // Resolve with the base64 string
      };
      reader.onerror = function (error) {
        reject(error);
      };
      reader.readAsDataURL(image[0]);
    });
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className="form-container">
        <div className="form-input-container">
          <label className="form-label" htmlFor="name">
            NAME:
          </label>
          <input
            id="name"
            className="text-input"
            type="text"
            placeholder="Name..."
            autoComplete="off"
            {...register("name", {
              required: "Enter name of the recipe",
            })}
          />
          <label className="error-text">{errors.name?.message}</label>
        </div>

        <div className="form-input-container">
          <label className="form-label" htmlFor="description">
            DESCRIPTION:
          </label>
          <input
            id="description"
            className="text-input"
            type="text"
            placeholder="Description..."
            autoComplete="off"
            {...register("description", {
              required: "Enter recipe description",
            })}
          />
          <label className="error-text">{errors.description?.message}</label>
        </div>

        <div className="form-input-container">
          <label className="form-label" htmlFor="main image">
            PREVIEW:
          </label>
          <input
            id="mainImage"
            className="file-input"
            type="file"
            accept="image/jpg"
            {...register("mainImage", {
              required: "Choose preview image",
            })}
          />
          <label className="error-text">{errors.mainImage?.message}</label>
        </div>

        <div>
          {Array.from({ length: stepCounter }).map((_, index) => (
            <>
              <div key={index} className="form-input-container">
                <label className="form-label" htmlFor={`stepImage-${index}`}>
                  #{index + 1} STEP IMAGE:
                </label>
                <input
                  id={`stepImage-${index}`}
                  className="file-input"
                  type="file"
                  accept="image/jpg"
                  {...register(`stepsImages[${index}]`, {
                    required: "Choose image for this step",
                  })}
                />
                <label className="error-text">
                  {errors.stepsImages?.[index]?.message}
                </label>
              </div>
              <div className="form-input-container">
                <label
                  className="form-label"
                  htmlFor={`stepDescription-${index}`}
                >
                  #{index + 1} STEP DESCRIPTION:
                </label>
                <input
                  id={`stepDescription-${index}`}
                  className="text-input"
                  type="text"
                  placeholder="Description..."
                  autoComplete="off"
                  {...register(`stepsDescriptions[${index}]`, {
                    required: "Enter step description",
                  })}
                />
                <label className="error-text">
                  {errors.stepsDescriptions?.[index]?.message}
                </label>
              </div>
            </>
          ))}
        </div>

        <div>
          {stepCounter >= 2 ? (
            <button className="button" type="button" onClick={removeStep}>
              REMOVE STEP
            </button>
          ) : null}
          {stepCounter <= 20 ? (
            <button className="button" type="button" onClick={addStep}>
              ADD STEP
            </button>
          ) : null}
        </div>

        <button
          disabled={loading}
          className="button submit-button"
          type="submit"
        >
          POST
        </button>
      </div>
    </form>
  );
};

export default RecipeInput;
