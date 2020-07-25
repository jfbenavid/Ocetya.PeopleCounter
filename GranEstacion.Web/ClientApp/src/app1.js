import React from "react";
import { Container } from "reactstrap";
import { Home } from "./components/home1";
import { createGlobalStyle } from "styled-components";

const GlobalStyle = createGlobalStyle`
  body {
		background-color: #393939;
		color: #fff;
  }
`;

const App = () => {
	return (
		<>
			<GlobalStyle whiteColor />
			<Container>
				<Home />
			</Container>
		</>
	);
};

export default App;
