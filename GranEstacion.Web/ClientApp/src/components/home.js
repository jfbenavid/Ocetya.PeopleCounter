import React, { useState, useEffect, useMemo } from "react";
import { Row, Col } from "reactstrap";

import { Counter } from "./counter";
import { LineChart } from "./line-chart";
import { Header } from "./header";
import { useInterval, config } from "../util";
import { SideInfo } from "./side-info";
import { ImgLogo } from "./styles/logo";
import { CenteredDiv, Spinner } from "./styles/spinner";

import endpoints from "../util/endpoints";
import { getLogData } from "../services/log";
import { getPeopleCount } from "../services/people";

import GELogo from "../images/GE.png";

export const Home = () => {
	const [currentPeople, setCurrentPeople] = useState(0);
	const [currentEntered, setCurrentEntered] = useState(0);
	const [currentGone, setCurrentGone] = useState(0);
	const [cameraChartData, setCameraChartData] = useState([]);
	const [peopleChartData, setPeopleChartData] = useState([]);

	const fetchData = async () => {
		setCameraChartData(await getLogData(endpoints.CAMERAS_LOG));
		setPeopleChartData(await getLogData(endpoints.PEOPLE_LOG));
		const { entered, gone, totalPeople } = await getPeopleCount();
		setCurrentEntered(entered);
		setCurrentGone(gone);
		setCurrentPeople(totalPeople);
	};

	useEffect(() => {
		fetchData();
	}, []);

	const refreshIntervalMilliseconds = config.REFRESH_INTERVAL;
	const cameraData = useMemo(() => cameraChartData, [cameraChartData]);
	const peopleData = useMemo(() => peopleChartData, [peopleChartData]);

	useInterval(() => {
		if (cameraChartData.length > 0 || peopleChartData.length > 0) {
			fetchData();
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
				<SideInfo entered={currentEntered} gone={currentGone} />
			</Header>
			<Row>
				<Col>
					{cameraChartData.length > 0 ? (
						<LineChart data={cameraData} />
					) : (
						<CenteredDiv>
							<Spinner />
						</CenteredDiv>
					)}
				</Col>
			</Row>
			<br />
			<Row>
				<Col>
					{peopleChartData.data && peopleChartData.data.length > 0 ? (
						<LineChart data={peopleData} />
					) : (
						<CenteredDiv>
							<Spinner />
						</CenteredDiv>
					)}
				</Col>
			</Row>
		</>
	);
};
