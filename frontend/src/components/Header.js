import React from "react";
import "./Header.css";
import { useNavigate } from "react-router-dom";

const Header = () => {
  const navigate = useNavigate();

  return (
    <div className="header-container">
      <label
        className="menu-option"
        onClick={() => {
          navigate("/");
        }}
      >
        COOK BOOK
      </label>
      <label
        className="menu-option"
        onClick={() => {
          navigate("/add-recipe");
        }}
      >
        ADD RECIPE
      </label>
    </div>
  );
};

export default Header;
