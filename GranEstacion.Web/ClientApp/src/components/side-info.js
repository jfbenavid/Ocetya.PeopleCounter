import React from "react";
import { CounterDiv } from "./styles/counter";

export const SideInfo = ({ entered = 0, gone = 0 }) => (
	<CounterDiv>
		<h3>Personas que han ingresado: {entered}</h3>
		<h3>Personas que han salido: {gone}</h3>
	</CounterDiv>
);
