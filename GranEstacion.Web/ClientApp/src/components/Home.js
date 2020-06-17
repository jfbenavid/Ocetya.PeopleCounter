import React from "react";
import { Counter } from "./counter";
import { LineChart } from "./line-chart";
import { Logo } from "./logo";
import { Header } from "./header";

export const Home = () => {
	return (
		<>
			<Header>
				<Logo />
			</Header>
			<Counter />
			<LineChart />
		</>
	);
};
