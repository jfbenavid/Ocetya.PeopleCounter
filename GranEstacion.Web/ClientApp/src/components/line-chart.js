import React, { useMemo } from "react";
import { Chart } from "react-charts";
import {
	Container,
	ChartDiv,
	LeftChart,
	LeftChartTitle,
} from "./styles/line-chart";

export const LineChart = ({
	data,
	leftTitle = "Personas",
	bottomTitle = "Hora",
}) => {
	const axes = useMemo(
		() => [
			{ primary: true, type: "time", position: "bottom" },
			{ type: "linear", position: "left" },
			{ type: "linear", position: "right" },
		],
		[]
	);

	return (
		<Container>
			<LeftChart>
				<LeftChartTitle>{leftTitle}</LeftChartTitle>
				<ChartDiv>
					{data instanceof Object && Object.keys(data[0]).length > 0 && (
						<Chart data={data} axes={axes} tooltip dark />
					)}
				</ChartDiv>
			</LeftChart>
			{bottomTitle}
		</Container>
	);
};
