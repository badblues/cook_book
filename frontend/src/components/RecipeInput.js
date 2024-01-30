import React, { useContext, useState } from "react";
import "./RecipeInput.css";
import { useForm, Controller } from "react-hook-form";
import { ApiContext } from "../contexts/ApiContext";

const RecipeInput = () => {
  const { register, control, handleSubmit, formState } = useForm();
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
    <div className="form-container">
      <form onSubmit={handleSubmit(onSubmit)}>
        <div className="form">
          <div className="form-input-container">
            <p className="form-label" htmlFor="name">
              NAME:
            </p>
            <input
              id="name"
              className="name-input"
              type="text"
              placeholder="Name..."
              autoComplete="off"
              maxLength={50}
              {...register("name", {
                required: "Enter name of the recipe"
              })}
            />
            <p className="error-text">{errors.name?.message}</p>
          </div>

          <div className="form-input-container">
            <label className="form-label" htmlFor="description">
              DESCRIPTION:
            </label>
            <Controller
              name="description"
              control={control}
              defaultValue=""
              render={({ field }) => (
                <div>
                  <textarea
                    {...field}
                    id="description"
                    className="text-input"
                    placeholder="Description..."
                    maxLength={400}
                  />
                </div>
              )}
            />

            <p className="error-text">{errors.description?.message}</p>
          </div>

          <div className="form-input-container">
            <label className="form-label" htmlFor="mainImage">
              PREVIEW:
            </label>
            <input
              id="mainImage"
              className="file-input"
              type="file"
              accept=".jpg"
              {...register("mainImage", {
                required: "Choose preview image",
              })}
            />
            <p className="error-text">{errors.mainImage?.message}</p>
          </div>

          <div>
            {Array.from({ length: stepCounter }).map((_, index) => (
              <div key={index}>
                <div className="form-input-container">
                  <label className="form-label" htmlFor={`stepImage-${index}`}>
                    STEP {index + 1} IMAGE:
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
                  <p className="error-text">
                    {errors.stepsImages?.[index]?.message}
                  </p>
                </div>
                <div className="form-input-container">
                  <label
                    className="form-label"
                    htmlFor={`stepDescription-${index}`}
                  >
                    STEP {index + 1} DESCRIPTION:
                  </label>
                  <Controller
                    name={`stepsDescriptions[${index}]`}
                    control={control}
                    defaultValue=""
                    render={({ field }) => (
                      <textarea
                        {...field}
                        id={`stepDescription-${index}`}
                        className="text-input"
                        placeholder="Description..."
                        maxLength={400}
                      />
                    )}
                  />

                  <p className="error-text">
                    {errors.stepsDescriptions?.[index]?.message}
                  </p>
                </div>
              </div>
            ))}
          </div>

          <div className="buttons-container">
            <div>
              {stepCounter >= 2 ? (
                <button className="default-button" type="button" onClick={removeStep}>
                  REMOVE STEP
                </button>
              ) : null}
              {stepCounter <= 20 ? (
                <button className="default-button" type="button" onClick={addStep}>
                  ADD STEP
                </button>
              ) : null}
            </div>
            <button
              disabled={loading}
              className="default-button submit-button"
              type="submit">
              POST
            </button>
          </div>
        </div>
      </form>
    </div>
  );
};

export default RecipeInput;
