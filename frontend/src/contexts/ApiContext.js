import React, { createContext, Component } from "react";
import RecipeApiService from "../services/RecipeApiService";

export const ApiContext = createContext({});

export class ApiContextProvider extends Component {
  constructor(props) {
    super(props);
    this.state = {
      recipeApiService: new RecipeApiService(),
      imagesApiUrl: "http://localhost:5055/images/",
    };
  }

  render() {
    return (
      <ApiContext.Provider value={this.state}>
        {this.props.children}
      </ApiContext.Provider>
    );
  }
}
