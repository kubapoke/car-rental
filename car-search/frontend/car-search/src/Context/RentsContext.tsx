import React, { createContext, useContext, useState } from 'react';


export interface Rent {
    brand: string,
    model: string,
    startDate: string,
    endDate: string
}


interface RentsContextProps {
    rents: Rent[];
    setRents: React.Dispatch<React.SetStateAction<Rent[]>>;
    fetchRents: () => Promise<void>;
}

const RentsContext = createContext<RentsContextProps | undefined>(undefined);

export const RentsProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [rents, setRents] = useState<Rent[]>([]);

    // fetch offers based on filters
    const fetchRents = async () => {
        const token = sessionStorage.getItem('authToken');
        const headers = {
            'Content-Type': 'application/json',
            ...(token && { Authorization: `Bearer ${token}` }),
        };
        
        const apiUrl = `${import.meta.env.VITE_SERVER_URL}/api/Rents/get-user-rents`;

        try {
            const res = await fetch(apiUrl, {method: 'GET', headers: headers});
            const data : Rent[]= await res.json();
            setRents(data);
        }
        catch (error){
            console.log('Error fetching data', error);
        }
    };

    return (
        <RentsContext.Provider value={{ rents, setRents, fetchRents }}>
            {children}
        </RentsContext.Provider>
    );
};

export const useRents = (): RentsContextProps => {
    const context = useContext(RentsContext);
    if (!context) {
        throw new Error('useRents must be used within an RentsProvider');
    }
    return context;
};
