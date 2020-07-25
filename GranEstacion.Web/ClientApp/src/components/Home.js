import React, { useState, useMemo } from "react";
import { Row, Col } from "reactstrap";

import { Counter } from "./counter";
import { LineChart } from "./line-chart";
import { Header } from "./header";
import { useInterval, config } from "../util";
import { SideInfo } from "./side-info";
import { ImgLogo } from "./styles/logo";

import GELogo from "../images/GE.png";

export const Home = () => {
	const [currentPeople, setCurrentPeople] = useState(10);
	const [chartData, setChartData] = useState({
		label: "Personas",
		data: [[new Date(), currentPeople]],
	});
	const maxDataShown = config.MAX_DATA_SHOWN_IN_MINUTES * 60;
	const refreshIntervalMilliseconds = config.REFRESH_INTERVAL;

	const data = useMemo(
		() => [
			{
				...chartData,
			},
		],
		[chartData]
	);

	useInterval(() => {
		setCurrentPeople(Math.round(Math.random() * 90));
		if (chartData.data.length > maxDataShown) {
			chartData.data.shift();
			const newData = chartData.data;
			setChartData({
				...chartData,
				data: [...newData, [new Date(), currentPeople]],
			});
		} else {
			setChartData({
				...chartData,
				data: [...chartData.data, [new Date(), currentPeople]],
			});
		}
	}, refreshIntervalMilliseconds);

	return (
		<>
			<Header>
				<ImgLogo src={GELogo} alt="Gran Estacion" />
				<Counter
					currentCount={currentPeople}
					maxPeople={config.MAX_PEOPLE_ALLOWED}
				/>
				<SideInfo />
			</Header>
			<Row>
				<Col>
					<LineChart data={data} />
				</Col>
			</Row>
			<br />
			<Row>
				<Col>
					<LineChart data={data} />
				</Col>
				<Col>
					<LineChart data={data} />
				</Col>
			</Row>
		</>
	);
};
