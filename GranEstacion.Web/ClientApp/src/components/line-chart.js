import React, { useState, useMemo } from "react";
import { Chart } from "react-charts";

let interval = setInterval(() => updateChart(), 1000);

const changeInterval = (milliseconds) => {
	clearInterval(interval);
	interval = setInterval(() => updateChart, milliseconds);
};

const updateChart = () => {
	// fetch("/WeatherForecast")
	// 	.then((data) => data.json())
	// 	.then((data) => console.log(data));
};

export const LineChart = (props) => {
	const [chartData, setChartData] = useState({
		label: "Personas",
		data: [[new Date(), 1]],
	});

	const data = React.useMemo(
		() => [
			{
				...chartData,
			},
		],
		[chartData]
	);

	const axes = useMemo(
		() => [
			{ primary: true, type: "time", position: "bottom" },
			{ type: "linear", position: "left" },
			{ type: "linear", position: "right" },
		],
		[]
	);

	return (
		<div
			style={{
				width: "100%",
				height: "300px",
				background: "rgba(0, 27, 45, 0.9)",
				borderRadius: "3px",
				padding: "15px",
			}}
		>
			<div
				style={{
					height: "100%",
					width: "100%",
				}}
			>
				<Chart data={data} axes={axes} tooltip dark />
			</div>
			<button
				onClick={() => {
					setChartData({
						...chartData,
						data: [
							...chartData.data,
							[new Date(), Math.round(Math.random() * 90)],
						],
					});
				}}
			>
				test
			</button>
		</div>
	);
};
