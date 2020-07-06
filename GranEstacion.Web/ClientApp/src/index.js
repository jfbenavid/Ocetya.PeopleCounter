import "bootstrap/dist/css/bootstrap.css";
import React from "react";
import { render } from "react-dom";
import App from "./App";
import registerServiceWorker from "./registerServiceWorker";

const rootElement = document.getElementById("root");

render(<App />, rootElement);

registerServiceWorker();
