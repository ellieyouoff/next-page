using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NP.Data;
using NP.Models;
using NP.Recommender;
using NP.Search;

using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace NP.Controllers
{
    public class BookController : Controller
    {
        private readonly NPDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly PearsonCorrelation _pearson;
        private readonly CosineSimilarity _cosine;

        public BookController(NPDbContext context, UserManager<User> userManager,
            PearsonCorrelation pearson, CosineSimilarity cosine)
        {
            _context = context;
            _userManager = userManager;
            _pearson = pearson;
            _cosine = cosine;
        }
        public static Genre[] GetGenres()
        {
            Genre[] genres = (Genre[])Enum.GetValues(typeof(Genre));
            return genres;
        }

        //Rating
        [HttpPost]
        public async Task<IActionResult> RegisterRating(string rating, string? id)
        {
            // Parse the rating value to an integer
            int ratingValue = int.Parse(rating);
            var currentUser = _userManager.GetUserAsync(User).Result;

            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.CurrentlyHeldBy)
                .Include(b => b.PostedBy)
                .FirstOrDefaultAsync(m => m.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            var alreadyRated = await _context.Ratings.FirstOrDefaultAsync(r => r.UserId == currentUser.Id && r.BookId == id);

            if (alreadyRated != null)
            {
                var ratingsForBook = await _context.Ratings.Where(r => r.BookId == id).ToListAsync();
                ratingsForBook.Remove(alreadyRated);
                alreadyRated.RatingValue = ratingValue;
                ratingsForBook.Add(alreadyRated);
                var sum = 0.0;
                foreach (var value in ratingsForBook)
                {
                    sum += value.RatingValue;
                }

                book.Rating = sum / ratingsForBook.Count();
                _context.Books.Update(book);
                _context.Ratings.Update(alreadyRated);


            }
            else
            {
                book.RatedCount = book.RatedCount + 1;

                var ratingsForBook = await _context.Ratings.Where(r => r.BookId == id).ToListAsync();


                // Create a new rating object
                var newRating = new Rating
                {
                    UserId = currentUser.Id,
                    BookId = id,
                    CategoryId = (int)book.Genre,
                    RatingValue = ratingValue
                };

                ratingsForBook.Add(newRating);
                var sum = 0.0;
                foreach (var value in ratingsForBook)
                {
                    sum += value.RatingValue;
                }

                book.Rating = sum / ratingsForBook.Count();

                if (book.Ratings == null)
                {
                    book.Ratings = new List<Rating>();
                }

                book.Ratings.Add(newRating);

                // Add the rating to the database
                _context.Ratings.Add(newRating);
                _context.Books.Update(book);
            }

            await _context.SaveChangesAsync();



            // Redirect back to the book details page
            return RedirectToAction("Details", "Book", new { id = id });

        }



            //Cosine Recommender
            public IActionResult CosineRecommendations()
        {
            var currentUser = _userManager.GetUserAsync(User).Result;
            var recommender = new Recommender.Recommender(_pearson, _cosine);
            var recommendedBooks = recommender.CosineRecommender(currentUser.Id);

            return View("CosineRecommendations", recommendedBooks);
        }

        //Pearson Recommender
        public IActionResult PearsonRecommendations()
        {
            var currentUser = _userManager.GetUserAsync(User).Result;
            var recommender = new Recommender.Recommender(_pearson, _cosine);
            var recommendedBooks = recommender.PearsonRecommender(currentUser.Id);

            return View("PearsonRecommendations", recommendedBooks);
        }


        // GET: Book
        public async Task<IActionResult> Index()
        {
            var nPDbContext = _context.Books.Include(b => b.CurrentlyHeldBy).Include(b => b.PostedBy);
            return View(await nPDbContext.ToListAsync());
        }



        // GET: Book/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.CurrentlyHeldBy)
                .Include(b => b.PostedBy)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }


            return View(book);
        }

        // GET: Book/Create
        [HttpGet]
        public IActionResult Create()
        {
            var book = new Book();

            return View(book);
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,Title,Description,Author,Published,Genre,Difficulty,NumberOfPages,Price,ISBN,PostedDate,Img,CurrentlyAtId,PostedById")] Book book)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            book.BookId = GenerateID();
            book.PostedById = currentUser.Id;
            book.PostedBy = currentUser;
            book.PostedDate = System.DateTime.Now;
            book.CurrentlyHeldById = currentUser.Id;
            book.CurrentlyHeldBy = currentUser;

            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["CurrentlyAtId"] = new SelectList(_context.Users, "Id", "Id", book.CurrentlyHeldById);
            ViewData["PostedById"] = new SelectList(_context.Users, "Id", "Id", book.PostedById);
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BookId,Title,Description,Author,Published,Genre,Difficulty,NumberOfPages,Price,ISBN,PostedDate,Img,CurrentlyAtId,PostedById")] Book book)
        {
            
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CurrentlyAtId"] = new SelectList(_context.Users, "Id", "Id", book.CurrentlyHeldById);
            ViewData["PostedById"] = new SelectList(_context.Users, "Id", "Id", book.PostedById);
            return View(book);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.CurrentlyHeldBy)
                .Include(b => b.PostedBy)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'NPDbContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        private bool BookExists(string id)
        {
          return (_context.Books?.Any(e => e.BookId == id)).GetValueOrDefault();
        }

        public string GenerateID()
        {
            return Guid.NewGuid().ToString("N");
        }

        //POST: This method allows to transfer book to a location
        public async Task<IActionResult> LeaveAtLocation(string bookId, string locationId, string userId) {

            var currentUser = _userManager.GetUserAsync(User).Result;

            if (_context.Books == null || _context.Locations == null || _context.Users == null)
            {
                return Problem("Entity set 'NPDbContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(bookId);
            var location = await _context.Locations.FindAsync(locationId);

            if (book != null && location != null && currentUser != null)
            {
                book.CurrentlyStoredAtId = location.Id;
                book.CurrentlyStoredAt = location;
                book.CurrentlyHeldById = null;
                book.CurrentlyHeldBy = null;
                currentUser.BooksStored.Remove(book);
                location.BooksStored.Append(book);

            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details(bookId)");
        }

        //POST: This method allows to take book from a location
        public async Task<IActionResult> TakeFromLocation(string bookId, string locationId)
        {

            var currentUser = _userManager.GetUserAsync(User).Result;

            if (_context.Books == null || _context.Locations == null || _context.Users == null)
            {
                return Problem("Entity set 'NPDbContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(bookId);
            var location = await _context.Locations.FindAsync(locationId);

            if (book != null && location != null && currentUser != null)
            {
                book.CurrentlyStoredAtId = null;
                book.CurrentlyStoredAt = null;
                book.CurrentlyHeldById = currentUser.Id;
                book.CurrentlyHeldBy = currentUser;
                currentUser.BooksStored.Append(book);
                location.BooksStored.Remove(book);

            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details(bookId)");
        }

        //POST: This method allows to take book from a location
        public async Task<IActionResult> UpdateRating(int rating, string bookId)
        {

            var currentUser = _userManager.GetUserAsync(User).Result;

            if (_context.Books == null || _context.Users == null)
            {
                return Problem("Entity set 'NPDbContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(bookId);
            

            if (book != null && currentUser != null)
            {
                book.RatedCount += 1;
                book.Rating = book.Rating + rating / book.RatedCount;
                Rating nRating = new Rating();
                nRating.BookId = bookId;
                nRating.UserId = currentUser.Id;
                nRating.RatingValue = rating;

                _context.Ratings.Append(nRating);

            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details(bookId)");
        }

        // GET: Books/Search
        public IActionResult Search(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return RedirectToAction(nameof(Index));
            }

            var searchResults = KMP.Search(searchTerm, _context);

            return View("Search", searchResults);
        }


    }
}
