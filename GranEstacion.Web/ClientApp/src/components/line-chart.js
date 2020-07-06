import React, { useMemo } from "react";
import { Chart } from "react-charts";

export const LineChart = ({ data }) => {
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
				{data instanceof Object && Object.keys(data[0]).length > 0 && (
					<Chart data={data} axes={axes} tooltip dark />
				)}
			</div>
		</div>
	);
};
