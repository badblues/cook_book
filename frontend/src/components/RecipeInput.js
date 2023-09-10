import React, { useContext, useState } from "react";
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
    setStepCounter(stepCounter + 1);
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
            Recipe name
          </label>
          <input
            id="name"
            className="form-input"
            type="text"
            placeholder="Name..."
            autoComplete="off"
            {...register("name", {
              required: "ERROR",
            })}
          />
          <label className="form-text">{errors.name?.message}</label>
        </div>

        <div className="form-input-container">
          <label className="form-label" htmlFor="description">
            Recipe description
          </label>
          <input
            id="description"
            className="form-input"
            type="text"
            placeholder="Description..."
            autoComplete="off"
            {...register("description", {
              required: "ERROR",
            })}
          />
          <label className="form-text">{errors.description?.message}</label>
        </div>

        <div className="form-input-container">
          <label className="form-label" htmlFor="main image">
            main Image
          </label>
          <input
            id="mainImage"
            className="form-input"
            type="file"
            accept="image/jpg"
            {...register("mainImage", {
              required: "ERROR",
            })}
          />
          <label className="form-text">{errors.mainImage?.message}</label>
        </div>

        <div>
          {Array.from({ length: stepCounter }).map((_, index) => (
            <>
              <div key={index} className="form-input-container">
                <label className="form-label" htmlFor={`stepImage-${index}`}>
                  stepImage {index + 1}
                </label>
                <input
                  id={`stepImage-${index}`}
                  className="form-input"
                  type="file"
                  accept="image/jpg"
                  {...register(`stepsImages[${index}]`, {
                    required: "ERROR",
                  })}
                />
                <label className="form-text">
                  {errors.stepsImages?.[index]?.message}
                </label>
              </div>
              <div className="form-input-container">
                <label
                  className="form-label"
                  htmlFor={`stepDescription-${index}`}
                >
                  Step description {index + 1}
                </label>
                <input
                  id={`stepDescription-${index}`}
                  className="form-input"
                  type="text"
                  placeholder="Description..."
                  autoComplete="off"
                  {...register(`stepsDescriptions[${index}]`, {
                    required: "ERROR",
                  })}
                />
                <label className="form-text">
                  {errors.stepsDescriptions?.[index]?.message}
                </label>
              </div>
            </>
          ))}
        </div>

        <div>
          <button type="button" onClick={addStep}>
            ADD STEP
          </button>
        </div>

        <button disabled={loading} className="button form-button" type="submit">
          POST
        </button>
      </div>
    </form>
  );
};

export default RecipeInput;
