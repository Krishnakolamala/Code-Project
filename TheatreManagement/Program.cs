using System;
using System.Collections.Generic;
using System.Text;

namespace TheatreManagement
{
  class Program
    {
        static void Main(string[] args)
        {
            List<RowSeating> rowSeatingData = new List<RowSeating>();
            List<TicketRequest> ticketRequests = new List<TicketRequest>();
            try
            {
                //Configure the theater with Rows and Sections in rows information
                TheaterSeatingConfiguration(rowSeatingData);

                //Calculate total seating capacity of theater
                int theaterCapacity = TheaterCapacity(rowSeatingData);

                //Patron seat requests
                PatronRequests(ticketRequests);

                //Allocate seats
                StringBuilder seatAllocationDetails = SeatAllocation(rowSeatingData, ticketRequests, theaterCapacity);

                Console.WriteLine();
                Console.WriteLine("Seat allotment Information");
                Console.WriteLine(seatAllocationDetails.ToString());

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Occured. Please try again. Error Message - " + ex.Message);
                Console.Read();
            }
            finally
            {
                rowSeatingData = null;
                ticketRequests = null;
            }

        }
        /// <summary>
        /// Configure the theater from user input rows and section data 
        /// </summary>
        /// <param name="rowSeatingData"></param>
        private static void TheaterSeatingConfiguration(List<RowSeating> rowSeatingData)
        {
            Console.WriteLine("Enter Number of Rows in Theater");
            int rows = 0;
            StringBuilder sbRowSection = new StringBuilder();
            if (Int32.TryParse(Console.ReadLine(), out rows))
            {
                if (rows > 0)
                {
                    Console.WriteLine("Enter the Row seat and Section information of theater");
                    for (int rowCount = 1; rowCount <= rows; rowCount++)
                    {
                        sbRowSection.Append(Console.ReadLine().ToString());
                        if (rowCount < rows)
                        {
                            sbRowSection.Append(Messages.NEW_LINE_CHARACTER);
                        }
                    }

                    RowSeating rowSeating = null;
                    int rowNo = 1;
                    Section section = null;

                    string[] rowSections = sbRowSection.ToString().Split(Messages.NEW_LINE_CHARACTER);
                    //store the user input in Object
                    foreach (string rowSection in rowSections)
                    {
                        List<Section> sections = new List<Section>();
                        rowSeating = new RowSeating();
                        rowSeating.Row = rowNo;
                        string[] sectionData = rowSection.Split(Messages.EMPTY_SPACE_CHARACTER);
                        int sectionNo = 1;
                        foreach (string seatsinSection in sectionData)
                        {
                            section = new Section() { SecetionNo = sectionNo, NoofSeats = Convert.ToInt32(seatsinSection) };
                            sections.Add(section);
                            sectionNo++;
                        }

                        rowSeating.SectionData = sections;
                        rowSeatingData.Add(rowSeating);
                        rowNo++;
                    }
                }
                else
                {
                    Console.WriteLine("Enter No of Rows greater than 0");
                }
            }
        }
        /// <summary>
        /// Calulates the seating capacity of a theater
        /// </summary>
        /// <param name="rowSeatingData"></param>
        /// <returns>Total no of seats</returns>
        private static int TheaterCapacity(List<RowSeating> rowSeatingData)
        {
            int seatingCapacity = 0;
            foreach (RowSeating rowSeating in rowSeatingData)
            {
                foreach (Section section in rowSeating.SectionData)
                {
                    seatingCapacity = seatingCapacity + section.NoofSeats;
                }
            }
            return seatingCapacity;
        }

        /// <summary>
        /// Patron Requests
        /// </summary>
        private static void PatronRequests(List<TicketRequest> ticketRequests)
        {
            Console.WriteLine("Enter Total Number of Patrons Requesting tickets");

            int totalPatronsRequested = 0;
            StringBuilder sbPatronsData = new StringBuilder();
            if (Int32.TryParse(Console.ReadLine(), out totalPatronsRequested))
            {
                if (totalPatronsRequested > 0)
                {
                    Console.WriteLine("Enter the Patron ticket reuests by Name <space> tickets");
                    for (int rowCount = 1; rowCount <= totalPatronsRequested; rowCount++)
                    {
                        sbPatronsData.Append(Console.ReadLine().ToString());
                        if (rowCount < totalPatronsRequested)
                        {
                            sbPatronsData.Append(Messages.NEW_LINE_CHARACTER);
                        }

                    }

                    TicketRequest ticketRequest = null;
                    string[] patronRequests = sbPatronsData.ToString().Split(Messages.NEW_LINE_CHARACTER);
                    //Store the Patrons data to object
                    foreach (string patronRequest in patronRequests)
                    {
                        string[] patronTicket = patronRequest.Split(Messages.EMPTY_SPACE_CHARACTER);
                        ticketRequest = new TicketRequest() { Name = patronTicket[0], TicketNo = Convert.ToInt32(patronTicket[1]) };
                        ticketRequests.Add(ticketRequest);
                    }
                }
                else
                {
                    Console.WriteLine("No Patren requested for Theater");
                }
            }
        }

        /// <summary>
        /// Seat Assignment to Patrons
        /// </summary>
        private static StringBuilder SeatAllocation(List<RowSeating> rowSeatingData, List<TicketRequest> ticketRequests, int theaterCapacity)
        {
            StringBuilder sbSeatAllocatonInfo = new StringBuilder();
            bool isTicketsRequestedMoreThanRowSection = false;
            //initialzing remaining seats to allocate with theater capacity
            int RemainingSeats = theaterCapacity;
            foreach (TicketRequest ticketRequest in ticketRequests)
            {
                if (ticketRequest.TicketNo > theaterCapacity || ticketRequest.TicketNo > RemainingSeats)
                {
                    sbSeatAllocatonInfo.Append(ticketRequest.Name + Messages.SORRY_PARTY_NOT_HANDLED).AppendLine();
                }
                else
                {
                    foreach (RowSeating rowSeating in rowSeatingData)
                    {
                        foreach (Section section in rowSeating.SectionData)
                        {
                            if (ticketRequest.TicketNo > section.NoofSeats)
                            {
                                isTicketsRequestedMoreThanRowSection = true;
                            }
                            else
                            {
                                isTicketsRequestedMoreThanRowSection = false;
                                section.NoofSeats = section.NoofSeats - ticketRequest.TicketNo;
                                //update remainingSeats to reflect the seats available after allocation
                                RemainingSeats = RemainingSeats - ticketRequest.TicketNo;
                                sbSeatAllocatonInfo.Append(ticketRequest.Name + Messages.ROW + rowSeating.Row + Messages.SECTION + section.SecetionNo).AppendLine();
                                //Once the seats are alloted for a Patron, break the loop t avoid further checking of sections
                                break;
                            }

                        }
                        if (!isTicketsRequestedMoreThanRowSection)
                        {
                            //Once the seats are alloted for a Patron, break the loop t avoid further checking of rows
                            break;
                        }
                    }

                    if (isTicketsRequestedMoreThanRowSection)
                    {
                        sbSeatAllocatonInfo.Append(ticketRequest.Name + Messages.SPLIT_PARTY).AppendLine();
                    }
                }
            }
             
            return sbSeatAllocatonInfo;
        }
    }
}
