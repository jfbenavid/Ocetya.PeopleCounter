import React, { useState } from "react";

export const Counter = () => {
	const [currentCount, setCurrentCount] = useState(12);

	return (
		<div>
			<h3>Personas en el Centro Comercial.</h3>
			<h1>{currentCount}</h1>
		</div>
	);
};
