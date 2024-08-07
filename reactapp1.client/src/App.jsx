import { useState, useEffect } from 'react';
import './App.css';
import { Modal, Button, Input, Checkbox, notification, DatePicker, Select } from 'antd';
import moment from 'moment';

const { RangePicker } = DatePicker;
const { Option } = Select;

export default function App() {
    const [users, setUsers] = useState([]);
    const [userFinder, setUserFinder] = useState('');
    const [selectedUser, setSelectedUser] = useState(null);
    const [loading, setLoading] = useState(false);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isUpdateModal, setIsUpdateModal] = useState(false);
    const [isUserActive, setIsUserActive] = useState(false);

    useEffect(() => {
        showUsers();
    }, []);

    const showUsers = async () => {
        setLoading(true);
        try {
            const response = await fetch('/user');
            const data = await response.json();
            setUsers(data);
        } catch (error) {
            notification.error({ message: 'Error fetching users', description: error.message });
        } finally {
            setLoading(false);
            setUserFinder('');
        }
    };

    const handleInput = (e) => {
        setUserFinder(e.target.value);
    };

    const showModal = (user = null) => {
        setSelectedUser(user || { name: '', email: '', isActive: false, absences: [] });
        setIsModalOpen(true);
        setIsUpdateModal(!!user);
        setIsUserActive(user?.isActive || false);
    };

    const hideModal = () => {
        setIsModalOpen(false);
        setSelectedUser(null);
    };

    const handleOk = async () => {
        try {
            const payload = { ...selectedUser, isActive: isUserActive };

            if (isUpdateModal && selectedUser) {
                await fetch(`/user/${selectedUser.id}`, {
                    method: 'PUT',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(payload),
                });
                notification.success({ message: 'User updated successfully' });
            } else {
                await fetch('/user', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(payload),
                });
                notification.success({ message: 'User created successfully' });
            }
            await showUsers();
            hideModal();
        } catch (error) {
            notification.error({ message: 'Error saving user', description: error.message });
        }
    };

    const handleCancel = () => {
        hideModal();
    };

    const handleDelete = (userId) => {
        Modal.confirm({
            title: 'Are you sure you want to delete this user?',
            okText: 'Yes',
            cancelText: 'No',
            onOk: async () => {
                try {
                    await fetch(`/user/${userId}`, {
                        method: 'DELETE',
                    });
                    notification.success({ message: 'User deleted successfully' });
                    await showUsers();
                } catch (error) {
                    notification.error({ message: 'Error deleting user', description: error.message });
                }
            },
        });
    };

    const handleInputChange = (e) => {
        setSelectedUser(prev => ({ ...prev, [e.target.name]: e.target.value }));
    };

    const handleAbsenceChange = (index, field, value) => {
        setSelectedUser(prev => {
            const absences = prev.absences ? [...prev.absences] : [];
            absences[index] = { ...absences[index], [field]: value };
            return { ...prev, absences };
        });
    };

    const addAbsence = () => {
        setSelectedUser(prev => ({
            ...prev,
            absences: prev.absences ? [...prev.absences, { type: 'Illnes', description: '', dateFrom: null, dateTo: null }] : [{ type: 'Illnes', description: '', dateFrom: null, dateTo: null }],
        }));
    };

    const removeAbsence = (index) => {
        setSelectedUser(prev => {
            const absences = prev.absences ? prev.absences.filter((_, i) => i !== index) : [];
            return { ...prev, absences };
        });
    };

    const downloadJSON = () => {
        const blob = new Blob([JSON.stringify(users, null, 2)], { type: 'application/json' });
        const url = URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = 'users.json';
        link.click();
        URL.revokeObjectURL(url);
    };

    const handleUserActivityChange = (e) => {
        setIsUserActive(e.target.checked);
    };

    const filteredUsers = userFinder
        ? users.filter(user => user.id === parseInt(userFinder, 10))
        : users;

    const contents = loading
        ? <p><em>Loading... Please wait.</em></p>
        : filteredUsers.length === 0
            ? <p><em>No users found. Click "Show all users" to fetch data.</em></p>
            : <table className="table table-striped" aria-labelledby="tableLabel">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Name</th>
                        <th>Email</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody>
                    {filteredUsers.map(user =>
                        <tr key={user.id}>
                            <td>{user.id}</td>
                            <td>{user.name}</td>
                            <td>{user.email}</td>
                            <td>
                                <Button className="updateButton" onClick={() => showModal(user)}>Update</Button>
                                <Button className="deleteButton" onClick={() => handleDelete(user.id)} danger>Delete</Button>
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>;

    return (
        <div>
            <h1 id="tableLabel">User Data</h1>
            <Input
                min={1}
                value={userFinder}
                onChange={handleInput}
                type="number"
                className="inputField"
                placeholder='Find by ID'
            />
            <Button onClick={showUsers} className="showUsers">Show all users</Button>
            <Button onClick={() => showModal()} className="createUser">Create New User</Button>
            <Button className="downloadJSON" onClick={downloadJSON}>Download .json</Button>
            <p>This component demonstrates fetching data from the server.</p>
            {contents}
            <Modal
                title={isUpdateModal ? "Update User" : "Create User"}
                visible={isModalOpen}
                onOk={handleOk}
                onCancel={handleCancel}
                afterClose={() => setSelectedUser(null)}
            >
                <Input
                    className="inputSetUser"
                    name="name"
                    value={selectedUser?.name || ''}
                    onChange={handleInputChange}
                    placeholder="Name"
                />
                <Input
                    name="email"
                    value={selectedUser?.email || ''}
                    onChange={handleInputChange}
                    placeholder="Email"
                />
                <div className="activeUsers">
                    <Checkbox checked={isUserActive} onChange={handleUserActivityChange} />
                    <label> User <strong>{selectedUser?.name}</strong> is {isUserActive ? 'active' : 'inactive'} </label>
                </div>
                {selectedUser?.absences?.map((absence, index) => (
                    <div key={index} className="absenceField">
                        <Select className='selectValue' value={absence.type} onChange={(value) => handleAbsenceChange(index, 'type', value)} style={{ width: 120 }}>
                            <Option value="Illnes">Illness</Option>
                            <Option value="Vacations">Vacations</Option>
                            <Option value="Other">Other</Option>
                        </Select>
                        <Input className='inputAbsField'
                            value={absence.description}
                            onChange={(e) => handleAbsenceChange(index, 'description', e.target.value)}
                            placeholder="Description"
                        />
                        <RangePicker
                            value={absence.dateFrom && absence.dateTo ? [moment(absence.dateFrom), moment(absence.dateTo)] : null}
                            onChange={(dates) => handleAbsenceChange(index, 'dateFrom', dates ? dates[0].toISOString() : null)}
                            onCalendarChange={(dates) => handleAbsenceChange(index, 'dateTo', dates ? dates[1].toISOString() : null)}
                        />
                        <Button onClick={() => removeAbsence(index)}>Remove</Button>
                    </div>
                ))}
                <Button onClick={addAbsence}>Add Absence</Button>
            </Modal>
        </div>
    );
}
