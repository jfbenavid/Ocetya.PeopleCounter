import React, { useState, useMemo } from "react";
import { Counter } from "./counter";
import { LineChart } from "./line-chart";
import { Logo } from "./logo";
import { Header } from "./header";
import { useInterval, config } from "../util";
import { SideInfo } from "./side-info";
import GELogo from "../images/GE.jpeg";
import { ImgLogo } from "./styles/logo";

export const Home = () => {
	const [currentPeople, setCurrentPeople] = useState(10);
	const [chartData, setChartData] = useState({
		label: "Personas",
		data: [],
	});

	useInterval(() => {
		setCurrentPeople(Math.round(Math.random() * 90));
		setChartData({
			...chartData,
			data: [...chartData.data, [new Date(), currentPeople]],
		});
	}, config.REFRESH_IN_SECONDS * 1000);

	const data = useMemo(
		() => [
			{
				...chartData,
			},
		],
		[chartData]
	);

	return (
		<>
			<Header>
				<ImgLogo src={GELogo} alt="Gran Estacion" />
				<Counter currentCount={currentPeople} />
				<SideInfo />
			</Header>
			<LineChart data={data} />
		</>
	);
};
