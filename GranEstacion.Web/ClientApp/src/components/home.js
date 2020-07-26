import React, { useState, useEffect, useMemo } from "react";
import { Row, Col } from "reactstrap";

import { Counter } from "./counter";
import { LineChart } from "./line-chart";
import { Header } from "./header";
import { useInterval, config } from "../util";
import { SideInfo } from "./side-info";
import { ImgLogo } from "./styles/logo";

import { getCamerasLog } from "../services/cameras";

import GELogo from "../images/GE.png";

export const Home = () => {
	const [currentPeople, setCurrentPeople] = useState(10);
	const [cameraChartData, setCameraChartData] = useState([]);

	const fetchData = async () => {
		const result = await getCamerasLog();
		setCameraChartData(result);
	};

	useEffect(() => {
		fetchData();
	}, []);

	const refreshIntervalMilliseconds = config.REFRESH_INTERVAL;
	const data = useMemo(() => cameraChartData, [cameraChartData]);

	useInterval(() => {
		if (cameraChartData.length > 0) {
			setCurrentPeople(Math.round(Math.random() * 90));
			fetchData();
		}
	}, refreshIntervalMilliseconds);

	return cameraChartData.length === 0 ? (
		<div>loading...</div>
	) : (
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
