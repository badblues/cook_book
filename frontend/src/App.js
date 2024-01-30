import "./App.css";
import RecipeInput from "./components/RecipeInput";
import Recipes from "./components/Recipes";
import RecipePage from "./components/RecipePage";
import Header from "./components/Header";
import Footer from "./components/Footer";
import { Routes, Route, Navigate } from "react-router-dom";

function App() {
  return (
    <div className="page">
      <Header />
      <div className="page-contents">
        <Routes>
          <Route element={<Recipes />} path="/" />
          <Route element={<RecipeInput />} path="/add-recipe" />
          <Route element={<RecipePage />} path="/recipes/:id"/>
          <Route path="/*" element={<Navigate to="/" />} />
        </Routes>
      </div>
      <Footer />
    </div>
  );
}

export default App;
