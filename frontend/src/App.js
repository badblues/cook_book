import "./App.css";
import RecipeInput from "./components/RecipeInput";
import Recipes from "./components/Recipes";
import RecipePage from "./components/RecipePage";
import Header from "./components/Header";
import Footer from "./components/Footer";
import { Routes, Route, Navigate } from "react-router-dom";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import NotFoundPage from "./components/NotFoundPage";

function App() {
  return (
    <div className="page">
      <Header />
      <div className="page-contents">
        <Routes>
          <Route element={<Recipes />} path="/" />
          <Route element={<RecipeInput />} path="/add-recipe" />
          <Route element={<RecipePage />} path="/recipes/:id"/>
          <Route element={<NotFoundPage />} path="/not-found" />
          <Route path="/*" element={<Navigate to="/" />} />
        </Routes>
      </div>
      <Footer />
      <ToastContainer />
    </div>
  );
}

export default App;
