window.JSFunction = (name, contributors, maxSize) => {
	Highcharts.chart('container', {
		chart: {
			type: 'packedbubble',
			height: '100%',
			backgroundColor: {
				linearGradient: [0, 0, 500, 500],
				stops: [
					[0, 'rgb(255, 255, 255)'],
					[1, 'rgb(240, 240, 255)']
				]
			}
		},
		title: {
			text: name,
			align: 'center'
		},
		tooltip: {
			useHTML: true,
			pointFormat: '<b>{point.name}:</b> {point.value} commits'
		},
		plotOptions: {
			packedbubble: {
				minSize: '10%',
				maxSize: maxSize + '%',
				zMin: 0,
				zMax: 1000,
				layoutAlgorithm: {
					splitSeries: false,
					gravitationalConstant: 0.02
				}
			}
		},
		series: [{
			name: name,
			data: contributors
		}]
	});
};