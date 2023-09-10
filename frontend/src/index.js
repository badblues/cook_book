import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import { ApiContextProvider } from "./contexts/ApiContext";
import { BrowserRouter } from "react-router-dom";

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <ApiContextProvider>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </ApiContextProvider>
);
