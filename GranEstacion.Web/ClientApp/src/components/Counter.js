import React from "react";
import { CounterDiv } from "./styles/counter";

export const Counter = ({ currentCount }) => {
	return (
		<CounterDiv>
			<h3>Personas en el Centro Comercial.</h3>
			<h1>{currentCount}</h1>
		</CounterDiv>
	);
};
