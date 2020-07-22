import React from "react";
import { CounterDiv, MaxDiv, H1, H4, H5, Span } from "./styles/counter";

export const Counter = ({ currentCount, maxPeople }) => {
	return (
		<CounterDiv>
			<H4>Personas en el Centro Comercial</H4>
			<H1 max={maxPeople}>{currentCount}</H1>
			<MaxDiv>
				<Span>Maximo aforo</Span>
				<H5>{maxPeople}</H5>
			</MaxDiv>
		</CounterDiv>
	);
};
