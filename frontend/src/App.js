import "./App.css";
import RecipeInput from "./components/RecipeInput";
import Recipes from "./components/Recipes";
import Header from "./components/Header";
import Footer from "./components/Footer";
import { Routes, Route } from "react-router-dom";

function App() {
  return (
    <div className="page">
      <Header />
      <div className="page-contents">
        <Routes>
          <Route element={<Recipes />} path="/" />
          <Route element={<RecipeInput />} path="/add-recipe" />
        </Routes>
      </div>
      <Footer />
    </div>
  );
}

export default App;
