import React from "react";
import { CounterDiv } from "./styles/counter";

export const SideInfo = ({ entered = 0, gone = 0 }) => (
	<CounterDiv>
		<h3>Ingreso: {entered}</h3>
		<h3>Salida: {gone}</h3>
	</CounterDiv>
);
