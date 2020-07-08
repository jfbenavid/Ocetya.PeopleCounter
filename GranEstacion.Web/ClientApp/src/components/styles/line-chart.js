import styled from "styled-components";

export const Container = styled.div`
	background: rgba(0, 27, 45, 0.9);
	border-radius: 3px;
	color: white;
	display: flex;
	flex-direction: column;
	align-items: center;
	height: 300px;
	padding: 15px 15px 0 0;
	width: 100%;
`;

export const ChartDiv = styled.div`
	height: 100%;
	margin-left: -10px;
	width: 100%;
`;

export const LeftChartTitle = styled.p`
	/* transform-origin: 0 0; */
	transform: rotate(270deg);
	color: white;
	margin-left: -18px;
	margin-bottom: 0;
`;

export const LeftChart = styled.div`
	display: flex;
	/* justify-content: space-between; */
	align-items: center;
	width: 100%;
	height: 100%;
`;
