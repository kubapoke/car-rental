import React, { createContext, useContext, useState } from 'react';

export interface Offer {
    carId: number;
    brand: string;
    model: string;
    email: string;
    price: number;
    conditions: string;
    companyName: string;
    startDate: string;
    endDate: string;
}

interface OffersContextProps {
    offers: Offer[];
    setOffers: React.Dispatch<React.SetStateAction<Offer[]>>;
    fetchOffers: (filters: Record<string, string>) => Promise<void>;
}

const OffersContext = createContext<OffersContextProps | undefined>(undefined);

export const OffersProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [offers, setOffers] = useState<Offer[]>([]);

    // fetch offers based on filters
    const fetchOffers = async (filters: Record<string, string>) => {
        const token = sessionStorage.getItem('authToken');
        const headers = {
            'Content-Type': 'application/json',
            ...(token && { Authorization: `Bearer ${token}` }),
        };

        const queryParams = new URLSearchParams(filters).toString();
        const apiUrl = `${import.meta.env.VITE_SERVER_URL}/api/OffersForward/offer-list?${queryParams}`;

        try {
            const res = await fetch(apiUrl, { method: 'GET', headers });
            const data: Offer[] = await res.json();
            setOffers(data);
        } catch (error) {
            console.error('Error fetching offers:', error);
        }
    };

    return (
        <OffersContext.Provider value={{ offers, setOffers, fetchOffers }}>
            {children}
        </OffersContext.Provider>
    );
};

export const useOffers = (): OffersContextProps => {
    const context = useContext(OffersContext);
    if (!context) {
        throw new Error('useOffers must be used within an OffersProvider');
    }
    return context;
};
