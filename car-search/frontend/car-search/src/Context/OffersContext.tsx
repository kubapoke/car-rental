import React, {createContext, useCallback, useContext, useState} from 'react';

export interface Offer {
    offerId: string;
    brand: string;
    model: string;
    email: string;
    price: number;
    conditions: string;
    location: string;
    companyName: string;
    startDate: string;
    endDate: string;
}

interface OffersContextProps {
    offers: Offer[];
    setOffers: React.Dispatch<React.SetStateAction<Offer[]>>;
    fetchOffers: (filters: Record<string, string>, page?: number, pageSize?: number) => Promise<void>;
    page: number;
    pageSize: number;
    totalOffers: number;
    pageCount: number;
    setPage: React.Dispatch<React.SetStateAction<number>>;
    setPageSize: React.Dispatch<React.SetStateAction<number>>;
}

const OffersContext = createContext<OffersContextProps | undefined>(undefined);

export const OffersProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [offers, setOffers] = useState<Offer[]>([]);
    const [page, setPage] = useState(0);
    const [pageSize, setPageSize] = useState(6);
    const [totalOffers, setTotalOffers] = useState(0);
    const [pageCount, setPageCount] = useState(0);

    // fetch offers based on filters
    const fetchOffers = useCallback(async (filters: Record<string, string>, page = 0, pageSize = 6) => {
        const token = sessionStorage.getItem('authToken');
        const headers = {
            'Content-Type': 'application/json',
            ...(token && { Authorization: `Bearer ${token}` }),
        };

        const queryParams = new URLSearchParams({
            ...filters,
            page: page.toString(),
            pageSize: pageSize.toString(),
        }).toString();
        const apiUrl = `${import.meta.env.VITE_SERVER_URL}/api/OffersForward/offer-list?${queryParams}`;

        const controller = new AbortController();
        const signal = controller.signal;

        try {
            const res = await fetch(apiUrl, { method: 'GET', headers, signal });
            const data = await res.json();
            setOffers(data.offers ?? []);
            setPage(data.page);
            setPageSize(data.pageSize);
            setTotalOffers(data.totalOffers);
            setPageCount(data.pageCount);
        } catch (error) {
            if (error instanceof DOMException && error.name === "AbortError") {
                console.log("Fetch aborted");
            } else {
                console.error("Error fetching offers:", error);
            }
        }

        controller.abort();
    }, []);

    return (
        <OffersContext.Provider
            value={{
                offers,
                setOffers,
                page,
                pageSize,
                totalOffers,
                pageCount,
                setPage,
                setPageSize,
                fetchOffers,
            }}>
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
