import React from 'react';
import { Bar } from 'react-chartjs-2';
import { Chart as ChartJS, CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend } from 'chart.js';

ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

const UserChart = ({ users }) => {
    const activeUsers = users.filter(user => user.isActive).length;
    const inactiveUsers = users.length - activeUsers;
    const data = {
        labels: ['Active Users', 'Inactive Users'],
        datasets: [
            {
                label: '',
                data: [activeUsers, inactiveUsers],
                backgroundColor: ['#36a2eb', '#ff6384'],
            },
        ],
    };

    return (
        <div>
            <h2>User Activity</h2>
            <Bar data={data} />
        </div>
    );
};

export default UserChart;
